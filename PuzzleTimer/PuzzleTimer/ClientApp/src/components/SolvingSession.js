import React, { Component } from 'react';
import { Puzzle } from './Puzzle';
import { User } from './User';
import eventBus from './EventBus';

export class SolvingSession extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            solvingSession: {},
            solvingSessions: [],
            puzzleId: '',
            showAddUser: false,
            totalTime: ''
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);
        this.createSession = this.createSession.bind(this);
        this.completeSession = this.completeSession.bind(this);
        this.addUser = this.addUser.bind(this);
        this.getTotalTime = this.getTotalTime.bind(this);
        this.dispatchTimerEvent = this.dispatchTimerEvent.bind(this);
    }

    componentDidMount() {
        this.getSolvingSession();
    }

    puzzleSearchResult(puzzle) {
        this.setState({ puzzleId: puzzle.id })
    }

    dispatchTimerEvent(running) {
        eventBus.dispatch("timerEvent", { running: running });
        setTimeout(() => this.getTotalTime(), 2000);
    }

    renderSession(solvingSession) {
        return (
            <div>
                <h4>{this.state.totalTime}</h4>
                <div className="btn-group g-3">
                    <button onClick={() => this.dispatchTimerEvent(true)} className="btn btn-success btn-lg p-3">Start Timers</button>
                    <button onClick={() => this.dispatchTimerEvent(false)} className="btn btn-danger btn-lg p-3">Stop Timers</button>
                </div>
                <br />
                <button onClick={() => this.completeSession()} className="btn btn-primary">Complete Session</button>
                <Puzzle puzzle={solvingSession.puzzle} showModal={false} />
            </div>
        )
    }

    renderCreateSession = () => {
        return (
            <div>
                <Puzzle puzzleSearchResult={(p) => this.puzzleSearchResult(p)} showModal={false} />
                <button onClick={() => this.createSession()} className="btn btn-primary"> Create New Session </button>
            </div>
        )
    }

    renderUsers() {
        let existingUsers;
        if (this.state.solvingSession !== null && this.state.solvingSession.users !== null && this.state.solvingSession.users.length > 0) {
            existingUsers = this.state.solvingSession.users.map((u) => {
                return <User key={u.id} user={u} solvingSessionId={this.state.solvingSession.id} >{u.name}</User>
            });
        }

        let showAddUser;
        if (this.state.showAddUser) {
            showAddUser = <User solvingSessionId={this.state.solvingSession.id} userSelect={(u) => this.addUser(u.id)} />;
        } else {
            showAddUser = <button onClick={() => this.setState({ showAddUser: true })} className="btn btn-primary">Add User</button>
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

    renderSessionList() {
        return (
            <div>
                {this.state.solvingSessions.map((s) => {
                    return (
                        <div key={s.id}>
                            <p>{s.puzzle.name}</p>
                            <p>{s.timeTaken}</p>
                        </div>
                    )
                })}
                {this.renderCreateSession()}
            </div>
        )
    }

    render() {
        let contents;
        let sessionList;

        if (this.state.loading) {
            contents = <p><em>Loading...</em></p>;
        } else if (this.state.solvingSessions.length > 0) {
            sessionList = this.renderSessionList();
        } else if (this.state.solvingSession === null) {
            contents = this.renderCreateSession();
        } else if (this.state.solvingSession !== null) {
            contents = this.renderSession(this.state.solvingSession);
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

    async getSolvingSession() {
        const url = new URL('solvingsession', window.location.origin);
        const response = await fetch(url);

        if (response.ok) {
            const data = await response.json();
            this.setState({ loading: false, solvingSession: data });
            this.getTotalTime();
        } else {
            this.getSolvingSessions();
        }
    }

    async getSolvingSessions() {
        const url = new URL('solvingSession/GetSessions', window.location.origin);
        const response = await fetch(url);

        if (response.ok) {
            const data = await response.json();
            this.setState({ loading: false, solvingSessions: data });
        } else {
            this.setState({ loading: false, solvingSession: null });
        }
    }

    async createSession() {
        const url = new URL('solvingsession/CreateSolvingSession?puzzleId=' + this.state.puzzleId, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();
        this.setState({ loading: false, solvingSession: data, solvingSessions: [] });
    }

    async completeSession() {
        const url = new URL('solvingSession/CompleteSession?sessionId=' + this.state.solvingSession.id, window.location.origin);
        const response = await fetch(url);

        const data = await response.text();

        this.setState({ loading: false, solvingSession: null, puzzleId: '' });

        alert(data);
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

        this.setState({ totalTime: data });
    }
}