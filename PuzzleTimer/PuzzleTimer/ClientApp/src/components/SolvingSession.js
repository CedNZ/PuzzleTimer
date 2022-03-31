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
    }

    componentDidMount() {
        this.getSolvingSession();
    }

    static renderSession(solvingSession) {
        return (
            <div>
                <p>{solvingSession.id}</p>
                <p>{solvingSession.started}</p>
                <Puzzle />
            </div>
        )
    }

    renderCreateSession = () => {
        return (
            <div>
                <Puzzle />
            </div>
        )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : SolvingSession.renderSession(this.state.solvingSession);

        return (
            <div>
                <h1>Solving Session</h1>
                {contents}
            </div>
        )
    }

    async getSolvingSession() {
        const response = await fetch('solvingsession');
        const data = await response.json();
        this.setState({ loading: false, solvingSession: data })
    }
}