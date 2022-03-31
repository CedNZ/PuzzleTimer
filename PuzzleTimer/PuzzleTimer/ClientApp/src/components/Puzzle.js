import React, { Component } from 'react';
import ReactModal from 'react-modal';

export class Puzzle extends Component {
    static displayName = Puzzle.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            puzzle: null,
            puzzleBarcode: '',
            puzzleName: '',
            pieceCount: '',
            showModal: false
        };

        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
    }

    handleOpenModal() {
        this.setState({ showModal: true });
    }

    handleCloseModal() {
        this.setState({ showModal: false });
    }

    componentDidMount() {

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

    renderPuzzleSearch() {
        return (
            <div>
                <input type="text" value={this.state.puzzleBarcode} onChange={(e) => { this.setState({ puzzleBarcode: e.target.value }); }} />
                <button onClick={() => { this.puzzleSearch(this) }}>Search</button>
            </div>
        )
    }

    renderCreatePuzzle() {
        return (
            <div>
                <input type="text" value={this.state.puzzleBarcode} onChange={(e) => { this.setState({ puzzleBarcode: e.target.value }); }} placeholder="barcode" />
                <br />
                <input type="text" value={this.state.puzzleName} onChange={(e) => { this.setState({ puzzleName: e.target.value }); }} placeholder="name" />
                <br />
                <input type="number" value={this.state.pieceCount} onChange={(e) => { this.setState({ pieceCount: e.target.value }); }} placeholder="pieces" />
                <br />
                <button onClick={() => this.createPuzzle(this)}>Create Puzzle</button>
            </div>
        )
    }

    render() {
        let contents = this.state.puzzle === null
            ? this.state.loading ? this.renderPuzzleSearch() : this.renderCreatePuzzle()
            : this.renderPuzzle(this.state.puzzle);

        return (
            <div>
                <button onClick={this.handleOpenModal}>Open Modal</button>
                <ReactModal isOpen={this.state.showModal}
                            contentLabel="Puzzle Modal" >
                    <h1>Puzzle</h1>
                    {contents}
                    <button onClick={this.handleCloseModal}>Close</button>
                </ReactModal>
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
                puzzleBarcode: puzzleComponent.state.puzzleBarcode
            });
        }
    }

    async createPuzzle(puzzleComponent) {
        var url = new URL('api/puzzle/createpuzzle', window.location.origin);

        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify({
                barcode: puzzleComponent.state.puzzleBarcode,
                name: puzzleComponent.state.puzzleName,
                pieceCount: puzzleComponent.state.pieceCount
            })
        });

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