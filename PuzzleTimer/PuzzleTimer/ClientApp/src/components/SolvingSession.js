import React, { Component } from 'react';
import { Puzzle } from './Puzzle';

export class SolvingSession extends Component {
    static displayName = SolvingSession.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            solvingSession: {}
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);
        this.createSession = this.createSession.bind(this);
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
            </div>
        )
    }

    renderCreateSession = () => {
        return (
            <div>
                <Puzzle puzzleSearchResult={(p) => this.puzzleSearchResult(p)} showModal={false} />
                <button onClick={() => this.createSession()}> Create New Session </button>
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

        return (
            <div>
                <h1>Solving Session</h1>
                {contents}
                <p>{this.state.puzzleId}</p>
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
}