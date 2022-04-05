import React, { Component } from 'react';
import ReactModal from 'react-modal';
import { PuzzleImage } from './PuzzleImage';

export class Puzzle extends Component {
    static displayName = Puzzle.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            puzzle: this.props.puzzle ?? null,
            puzzleBarcode: '',
            puzzleName: '',
            pieceCount: '',
            showModal: this.props.showModal ?? true
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);

        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.puzzleSearch = this.puzzleSearch.bind(this);
        this.createPuzzle = this.createPuzzle.bind(this);
    }

    handleOpenModal() {
        this.setState({ showModal: true });
    }

    handleCloseModal() {
        this.setState({ showModal: false });
    }

    puzzleSearchResult(puzzle) {
        if (this.props.puzzleSearchResult !== undefined) {
            this.props.puzzleSearchResult(puzzle);
        }
    }

    componentWillMount() {
        ReactModal.setAppElement('body');
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
        let contents;
        let buttonText;

        if (this.state.puzzle === null) {
            if (this.state.loading) {
                contents = this.renderPuzzleSearch();
            } else {
                contents = this.renderCreatePuzzle();
            }
            buttonText = 'Open Puzzle Search';
        } else {
            contents = this.renderPuzzle(this.state.puzzle);
            buttonText = `${this.state.puzzle.id} - ${this.state.puzzle.name}`;
        }


        return (
            <div className="puzzle">
                <button onClick={this.handleOpenModal}>{buttonText}</button>
                <ReactModal isOpen={this.state.showModal}
                            contentLabel="Puzzle Search" >
                    <h1>Puzzle</h1>
                    {contents}
                    <button onClick={this.handleCloseModal}>Close</button>
                </ReactModal>
                <PuzzleImage puzzleId={this.state.puzzle.id} sessionId={this.props.sessionId} />
            </div>
        )
    }

    async puzzleSearch() {
        var data = { barcode: this.state.puzzleBarcode };
        var url = new URL('api/puzzle/getpuzzle', window.location.origin);
        for (let k in data) {
            url.searchParams.append(k, data[k]);
        }

        const response = await fetch(url);

        if (response.ok) {
            let puzzleJson = await response.json();
            this.setState({
                loading: false,
                puzzle: puzzleJson,
                puzzleBarcode: this.state.puzzleBarcode
            });
            this.puzzleSearchResult(puzzleJson);
        } else {
            this.setState({
                loading: false,
                puzzle: null,
                puzzleBarcode: this.state.puzzleBarcode
            });
        }
    }

    async createPuzzle() {
        var url = new URL('api/puzzle/createpuzzle', window.location.origin);

        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify({
                barcode: this.state.puzzleBarcode,
                name: this.state.puzzleName,
                pieceCount: this.state.pieceCount
            })
        });

        if (response.ok) {
            let puzzleJson = await response.json();
            this.setState({
                loading: false,
                puzzle: puzzleJson,
                puzzleBarcode: this.state.puzzleBarcode
            });
        } else {
            this.setState({
                loading: false,
                puzzle: null,
                puzzleBarcode: ''
            });
        }
    }
}