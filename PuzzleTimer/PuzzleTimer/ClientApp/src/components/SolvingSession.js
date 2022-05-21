import React, { Component } from 'react';
import { Container, Row, Col } from 'react-bootstrap'
import Card from 'react-bootstrap/Card';
import { Puzzle } from './Puzzle';
import { User } from './User';
import eventBus from './EventBus';
import { DateTime, Duration } from 'luxon';

export class SolvingSession extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            solvingSession: {},
            solvingSessions: [],
            puzzleId: null,
            showAddUser: false,
            baseTotalTime: '',
            totalTime: Duration.fromMillis(0),
            timerStartTimes: [],
            tickInterval: -1
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);
        this.createSession = this.createSession.bind(this);
        this.completeSession = this.completeSession.bind(this);
        this.addUser = this.addUser.bind(this);
        this.getTotalTime = this.getTotalTime.bind(this);
        this.dispatchAllTimerEvent = this.dispatchAllTimerEvent.bind(this);
        this.handleTimerEvent = this.handleTimerEvent.bind(this);
        this.tick = this.tick.bind(this);
        this.filteredSessions = this.filteredSessions.bind(this);
    }

    componentDidMount() {
        this.getSolvingSession();
        eventBus.on("timerEvent", this.handleTimerEvent);
    }

    componentWillUnmount() {
        eventBus.remove("timerEvent", this.handleTimerEvent);
    }

    puzzleSearchResult(puzzle) {
        this.setState({ puzzleId: puzzle.id })
    }

    dispatchAllTimerEvent(running) {
        eventBus.dispatch("allTimerEvent", { running: running });
        setTimeout(() => this.getTotalTime(), 2000);
    }

    handleTimerEvent(data) {
        this.getTotalTime();
        if (data.start) {
            this.setState(prevState => {
                return { timerStartTimes: [...prevState.timerStartTimes, data] };
            });
            if (this.state.tickInterval < 0) {
                let interval = setInterval(() => this.tick(), 1000);
                this.setState({ tickInterval: interval });
            }
        } else {
            this.setState(prevState => {
                return { timerStartTimes: prevState.timerStartTimes.filter((item) => item.userId != data.userId) };
            });
            if (this.state.timerStartTimes.length === 0) {
                clearInterval(this.state.tickInterval);
                this.setState({ tickInterval: -1 });
            }
        }
    }

    tick() {
        let now = DateTime.now();

        let time = this.state.timerStartTimes.reduce((agg, next) => {
            return agg.plus(now.diff(next.start)).shiftTo('hours', 'minutes', 'seconds');
        }, this.state.baseTotalTime);

        this.setState({ totalTime: time });
    }

    filteredSessions() {
        let puzzleId = this.state.puzzleId;
        if (puzzleId === null) {
            if (this.state.solvingSession === null) {
                return this.state.solvingSessions;
            }
            puzzleId = this.state.solvingSession.puzzle.id;
        }

        return this.state.solvingSessions.filter(s => s.puzzle.id === puzzleId
            && s.id !== this.state.solvingSession?.id)
    }

    renderSession(solvingSession) {
        let buttons = (
            <div>
                <div className="btn-group g-3">
                    <button onClick={() => this.dispatchAllTimerEvent(true)} className="btn btn-success btn-lg p-3">Start Timers</button>
                    <button onClick={() => this.dispatchAllTimerEvent(false)} className="btn btn-danger btn-lg p-3">Stop Timers</button>
                </div>
                <br />
                <button onClick={() => this.completeSession()} className="btn btn-primary">Complete Session</button>
            </div>
        );
        if (solvingSession.completed !== null && solvingSession.completed !== undefined) {
            buttons = (
                <div className="btn-group">
                    <button onClick={() => {
                        this.setState({ solvingSession: null, puzzleId: null });
                        this.getSolvingSessions();
                    }} className="btn btn-primary">
                        Back to list
                    </button>
                    <button onClick={() => {
                        if (window.confirm('Are you sure you want to delete this session?')) {
                            this.deleteSession();
                        }
                    }} className="btn btn-danger">
                        Delete Session
                    </button>
                </div>
            );
        }

        return (
            <div>
                <h4>{this.state.totalTime.toFormat("h'h 'm'm 's's'")}</h4>
                <br />
                {buttons}
                <br />
                <br />
                <Puzzle puzzle={solvingSession.puzzle} showModal={false} />
                <br />
                <br />
                <Container fluid>
                    <Row>
                        {this.filteredSessions().map((s) => {
                            return (
                                <Col key={s.id}>
                                    {this.renderSessionCard(s, solvingSession.completed)}
                                </Col>
                            )
                        })}
                    </Row>
                </Container>
            </div>
        )
    }

    renderCreateSession = () => {
        return (
            <div>
                <Puzzle puzzleSearchResult={(p) => this.puzzleSearchResult(p)} showModal={false} />
                <button onClick={() => this.createSession()} className="btn btn-primary" disabled={this.state.puzzleId === null}> Create New Session </button>
            </div>
        )
    }

    renderUsers() {
        const completedSession = (this.state.solvingSession.completed !== null && this.state.solvingSession.completed !== undefined);

        let existingUsers;
        if (this.state.solvingSession !== null && this.state.solvingSession.users !== null && this.state.solvingSession.users.length > 0) {
            existingUsers = this.state.solvingSession.users.map((u) => {
                return <User key={u.id}
                    user={u}
                    solvingSessionId={this.state.solvingSession.id}
                    completed={completedSession}>
                    {u.name}
                </User>
            });
        }

        let showAddUser;
        if (this.state.showAddUser) {
            showAddUser = <User solvingSessionId={this.state.solvingSession.id} userSelect={(u) => this.addUser(u.id)} />;
        } else {
            showAddUser = <button onClick={() => this.setState({ showAddUser: true })} className="btn btn-primary">Add User</button>
        }

        if (completedSession) {
            showAddUser = null;
        }

        return (
            <div className="userContainer container">
                <div className="row">
                    {showAddUser}
                </div>
                <div className="row gx-5">
                    {existingUsers}
                </div>
            </div>
        )
    }

    renderSessionCard(s, clickable) {
        return (
            <Card key={s.id} style={{ width: '18rem' }} onClick={() => clickable ? this.getSolvingSession(s.id) : ''}>
                <Card.Img variant="top" src={`/image/getPic?id=${s?.image?.id}`} />
                <Card.Body>
                    <Card.Title>{s.puzzle.name}</Card.Title>
                    <Card.Text>
                        {Duration.fromISO(s.timeTaken).toHuman()}
                        <br />
                        {(s.puzzle.pieceCount / ((Duration.fromISO(s.timeTaken).toMillis()) / 1000 / 60)).toFixed(2)} pieces per minute
                        <br />
                        {s.users.map(u => u.name).join(', ')}
                        <br />
                        {new Date(s.completed).toLocaleDateString()}
                    </Card.Text>
                </Card.Body>
            </Card>
        )
    }

    renderSessionList() {
        return (
            <div>
                {this.renderCreateSession()}
                <Container fluid>
                    <Row>
                        {this.filteredSessions().map((s) => {
                            return (
                                <Col key={s.id}>
                                    {this.renderSessionCard(s, true)}
                                </Col>
                            )
                        })}
                    </Row>
                </Container>
            </div>
        )
    }

    render() {
        let contents;
        let sessionList;

        if (this.state.loading) {
            contents = <p><em>Loading...</em></p>;
        } else if (this.state.solvingSession !== null) {
            contents = this.renderSession(this.state.solvingSession);
        } else if (this.state.solvingSessions.length > 0) {
            sessionList = this.renderSessionList();
        } else if (this.state.solvingSession === null) {
            contents = this.renderCreateSession();
        } else {
            contents = <h1>Shit's fucked</h1>;
        }

        let userElement;
        if (this.state.solvingSession !== null && this.state.solvingSession.id !== undefined) {
            userElement = this.renderUsers();
        }

        return (
            <div className="solvingSession">
                {userElement}
                {sessionList}
                {contents}
                <br />
            </div>
        )
    }

    async getSolvingSession(sessionId) {
        const url = sessionId === undefined
            ? new URL('solvingsession', window.location.origin)
            : new URL('solvingsession/GetSession?sessionId=' + sessionId, window.location.origin);

        const response = await fetch(url);
        
        if (response.ok) {
            const data = await response.json();
            this.setState({ loading: false, solvingSession: data, totalTime: Duration.fromMillis(0) });
            this.getTotalTime();
        } else {
            this.setState({ loading: false, solvingSession: null, totalTime: Duration.fromMillis(0) });
        }
        this.getSolvingSessions();
    }

    async getSolvingSessions() {
        const url = new URL('solvingSession/GetSessions', window.location.origin);
        const response = await fetch(url);

        if (response.ok) {
            const data = await response.json();
            this.setState({ loading: false, solvingSessions: data });
        } else {
            this.setState({ loading: false, solvingSessions: [] });
        }
    }

    async createSession() {
        const url = new URL('solvingsession/CreateSolvingSession?puzzleId=' + this.state.puzzleId, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();
        this.setState({ loading: false, solvingSession: data });
    }

    async completeSession() {
        if (this.state.tickInterval > 0) {
            clearInterval(this.state.tickInterval);
            this.setState({ tickInterval: -1 });
        }

        const url = new URL('solvingSession/CompleteSession?sessionId=' + this.state.solvingSession.id, window.location.origin);
        const response = await fetch(url);

        const data = await response.text();

        this.setState({ loading: false, solvingSession: null, puzzleId: null });

        alert(data);

        this.getSolvingSessions();
    }

    async addUser(userId) {
        const url = new URL(`solvingSession/AddUser?sessionId=${this.state.solvingSession.id}&userId=${userId}`, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();

        this.setState({ solvingSession: data, showAddUser: false });
    }

    async getTotalTime() {
        const url = new URL('solvingSession/GetSessionTime?sessionId=' + this.state.solvingSession.id, window.location.origin);
        const response = await fetch(url);

        const data = await response.text();

        let time = Duration.fromISO(data);

        this.setState({ totalTime: time, baseTotalTime: time });
    }

    async deleteSession() {
        const url = new URL('solvingSession/DeleteSession?sessionId=' + this.state.solvingSession.id, window.location.origin);
        const response = await fetch(url);

        if (response.status === 200) {
            this.setState({ solvingSession: null });
            this.getSolvingSessions();
        }
    }
}