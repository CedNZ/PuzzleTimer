import React, { Component } from 'react';
import { Puzzle } from './Puzzle';
import { User } from './User';
import eventBus from './EventBus';

export class SolvingSession extends Component {
    static displayName = SolvingSession.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            solvingSession: {},
            puzzleId: '',
            showAddUser: false
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);
        this.createSession = this.createSession.bind(this);
        this.completeSession = this.completeSession.bind(this);
        this.addUser = this.addUser.bind(this);
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
    }

    renderSession(solvingSession) {
        return (
            <div>
                <p>{solvingSession.id}</p>
                <p>{solvingSession.started}</p>
                <Puzzle puzzle={solvingSession.puzzle} showModal={false} />
                <button onClick={() => this.completeSession()} className="btn btn-primary">Complete Session</button>
                <div className="btn-group">
                    <button onClick={() => this.dispatchTimerEvent(true)} className="btn btn-success">Start Timers</button>
                    <button onClick={() => this.dispatchTimerEvent(false)} className="btn btn-danger">Stop Timers</button>
                </div>
            </div>
        )
    }

    renderCreateSession = () => {
        if (this.state.puzzleId === '') {
            return (
                <div>
                    <Puzzle puzzleSearchResult={(p) => this.puzzleSearchResult(p)} showModal={false} />
                </div>
            )
        }
        else {
            return (
                <div>
                    <button onClick={() => this.createSession()} className="btn btn-primary"> Create New Session </button>
                </div>
            )
        }
    }

    renderUsers() {
        let existingUsers;
        if (this.state.solvingSession !== null && this.state.solvingSession.users !== undefined && this.state.solvingSession.users.length > 0) {
            existingUsers = this.state.solvingSession.users.map((u) => {
                return <User key={u.id} user={u} solvingSessionId={this.state.solvingSession.id} >{u.name}</User>
            });
        }

        let showAddUser;
        if (this.state.showAddUser) {
            showAddUser = <User solvingSessionId={this.state.solvingSession.id} userSelect={(u) => this.addUser(u.id)} />;
        } else {
            showAddUser = <button onClick={() => this.setState({ showAddUser: true })} className="btn btn-primary">+</button>
        }

        return (
            <div className="userContainer container">
                <div className="row gx-5">
                    {existingUsers}
                </div>
                <div className="row">
                    {showAddUser}
                </div>
            </div>
        )
    }

    render() {
        let contents;

        if (this.state.loading) {
            contents = <p><em>Loading...</em></p>;
        } else if (this.state.solvingSession === null) {
            contents = this.renderCreateSession();
        } else if (this.state.solvingSession !== null) {
            contents = this.renderSession(this.state.solvingSession);
        } else {
            contents = <h1>Shit's fucked</h1>;
        }

        let userElement;
        if (this.state.solvingSession !== null) {
            userElement = this.renderUsers();
        }

        return (
            <div className="solvingSession">
                <h1>Solving Session</h1>
                {contents}
                <br />
                {userElement}
            </div>
        )
    }

    async getSolvingSession() {
        const url = new URL('solvingsession', window.location.origin);
        const response = await fetch(url);

        if (response.ok) {
            const data = await response.json();
            this.setState({ loading: false, solvingSession: data });
        } else {
            this.setState({ loading: false, solvingSession: null });
        }
    }

    async createSession() {
        const url = new URL('solvingsession/CreateSolvingSession?puzzleId=' + this.state.puzzleId, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();
        this.setState({ loading: false, solvingSession: data });
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
}