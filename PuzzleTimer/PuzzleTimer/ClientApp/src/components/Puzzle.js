import React, { Component } from 'react';

export class Puzzle extends Component {
    static displayName = Puzzle.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            puzzle: null,
            puzzleBarcode: ''
        };
    }

    componentDidMount() {

    }

    renderPuzzleSearch() {
        return (
            <div>
                <input type="text" value={this.state.puzzleBarcode} onChange={(e) => { this.setState({ puzzleBarcode: e.target.value }); }} />
                <button onClick={() => { this.puzzleSearch(this) }}>Search</button>
            </div>
        )
    }

    renderPuzzle(puzzle) {
        return (
            <div>
                <p>{puzzle.id}</p>
                <p>{puzzle.name}</p>
                <p>{puzzle.barcode}</p>
                <p>{puzzle.pieceCount}</p>
            </div>
        )
    }

    render() {
        let contents = this.state.puzzle === null
            ? this.renderPuzzleSearch()
            : this.renderPuzzle(this.state.puzzle);

        return (
            <div>
                <h1>Puzzle</h1>
                {contents}
            </div>
        )
    }

    async puzzleSearch(puzzleComponent) {
        var data = { barcode: puzzleComponent.state.puzzleBarcode };
        var url = new URL('api/puzzle/getpuzzle', window.location.origin);
        for (let k in data) {
            url.searchParams.append(k, data[k]);
        }

        const response = await fetch(url);

        if (response.ok) {
            let puzzleJson = await response.json();
            puzzleComponent.setState({
                loading: false,
                puzzle: puzzleJson,
                puzzleBarcode: puzzleComponent.state.puzzleBarcode
            });
        } else {
            puzzleComponent.setState({
                loading: false,
                puzzle: null,
                puzzleBarcode: ''
            });
        }
    }
}