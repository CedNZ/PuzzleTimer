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
                <Puzzle puzzleSearchResult={(p) => this.puzzleSearchResult(p)} showModal={false} />
            </div>
        )
    }

    renderCreateSession = () => {
        return (
            <div>

            </div>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderSession(this.state.solvingSession);

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
        const data = await response.json();
        this.setState({ loading: false, solvingSession: data })
    }
}