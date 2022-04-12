import { isNumeric } from 'jquery';
import React, { Component } from 'react';
import ReactModal from 'react-modal';
import { PuzzleImage } from './PuzzleImage';

export class Puzzle extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            puzzle: this.props.puzzle ?? null,
            puzzleBarcode: '',
            puzzleName: '',
            pieceCount: '',
            showModal: this.props.showModal ?? true,
            puzzleSelection: []
        };

        this.puzzleSearchResult = this.puzzleSearchResult.bind(this);

        this.handleOpenModal = this.handleOpenModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handlePuzzleSearch = this.handlePuzzleSearch.bind(this);
        this.textPuzzleSearch = this.textPuzzleSearch.bind(this);
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

    componentDidMount() {
        ReactModal.setAppElement('body');
    }

    handlePuzzleSearch(input) {
        if (!isNumeric(input)) {
            this.textPuzzleSearch(input);
        }
    }

    setSelectedPuzzle(puzzle) {
        this.setState({
            loading: false,
            puzzle: puzzle,
            puzzleBarcode: puzzle.barcode,
            showModal: false
        });
        this.puzzleSearchResult(puzzle);
    }

    renderPuzzle(puzzle) {
        return (
            <div>
                <p>{puzzle.id}</p>
                <p>{puzzle.name}</p>
                <p>{puzzle.barcode}</p>
                <p>{puzzle.pieceCount}</p>
                <button onClick={() => this.setState({ loading: true, puzzle: null, puzzleBarcode: '' })} className="btn btn-primary" >Clear</button>
            </div>
        )
    }

    renderPuzzleSearch() {
        let puzzleSelect;
        if (this.state.puzzleSelection.length > 0) {
            puzzleSelect = (
                <ul>
                    {this.state.puzzleSelection.map((puzzle) => <li key={puzzle.id} value={puzzle.id} onClick={() => this.setSelectedPuzzle(puzzle)}>{puzzle.name}</li>)}
                </ul>
            )
        }

        return (
            <div>
                <input type="text" value={this.state.puzzleBarcode} onChange={(e) => {
                    this.setState({ puzzleBarcode: e.target.value });
                    this.handlePuzzleSearch(e.target.value);
                }} />
                <button onClick={() => { this.puzzleSearch(this) }}>Search</button>
                {puzzleSelect}
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
        let puzzleImage;

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
            puzzleImage = (<PuzzleImage puzzleId={this.state.puzzle?.id ?? null} sessionId={this.props.sessionId}></PuzzleImage>);
        }


        return (
            <div className="puzzle">
                {puzzleImage}
                <button onClick={this.handleOpenModal}>{buttonText}</button>
                <ReactModal isOpen={this.state.showModal}
                            contentLabel="Puzzle Search" >
                    <h1>Puzzle</h1>
                    {contents}
                    <button onClick={this.handleCloseModal}>Close</button>
                </ReactModal>
            </div>
        )
    }

    async textPuzzleSearch(text) {
        const url = new URL('api/puzzle/findPuzzlesByName?name=' + text, window.location.origin);
        const response = await fetch(url);

        if (response.status == 200) {
            const data = await response.json();
            this.setState({ puzzleSelection: data });
        } else {
            this.setState({ puzzleSelection: [] });
        }
    }

    async puzzleSearch() {
        var data = { barcode: this.state.puzzleBarcode };
        const url = new URL('api/puzzle/getpuzzle', window.location.origin);
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
        const url = new URL('api/puzzle/createpuzzle', window.location.origin);

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
