import React, { Component } from 'react';
import { Puzzle } from './Puzzle';
import { User } from './User';

export class SolvingSession extends Component {
    static displayName = SolvingSession.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            solvingSession: {},
            puzzleId: ''
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);
        this.createSession = this.createSession.bind(this);
        this.completeSession = this.completeSession.bind(this);
    }

    componentDidMount() {
        this.getSolvingSession();
    }

    puzzleSearchResult(puzzle) {
        this.setState({ puzzleId: puzzle.id })
    }

    renderSession(solvingSession) {
        return (
            <div>
                <p>{solvingSession.id}</p>
                <p>{solvingSession.started}</p>
                <p>{solvingSession.puzzle.id} - {solvingSession.puzzle.name}</p>
                <button onClick={() => this.completeSession()}>Complete Session</button>
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
                    <button onClick={() => this.createSession()}> Create New Session </button>
                </div>
            )
        }
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
            userElement = (
                <User solvingSessionId={this.state.solvingSession.id} />
            );
        }

        return (
            <div>
                <h1>Solving Session</h1>
                {contents}
                <br />
                {userElement}
            </div>
        )
    }

    async getSolvingSession() {
        const response = await fetch('solvingsession');

        if (response.ok) {
            const data = await response.json();
            this.setState({ loading: false, solvingSession: data });
        } else {
            this.setState({ loading: false, solvingSession: null });
        }
    }

    async createSession() {
        const response = await fetch('solvingsession/CreateSolvingSession?puzzleId=' + this.state.puzzleId);

        const data = await response.json();
        this.setState({ loading: false, solvingSession: data });
    }

    async completeSession() {
        const response = await fetch('solvingSession/CompleteSession?sessionId=' + this.state.solvingSession.id);

        const data = await response.text();

        this.setState({ loading: false, solvingSession: null, puzzleId: '' });

        alert(data);
    }
}