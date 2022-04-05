import React, { Component } from 'react';

export class PuzzleImage extends Component {
    static displayName = PuzzleImage.name;

    constructor(props) {
        super(props);
        this.state = {
            imgId: '',
            base64: '',
            images: [],
            fileName: ''
        }

        this.encodeImage = this.encodeImage.bind(this);
        this.upload = this.upload.bind(this);
        this.getImages = this.getImages.bind(this);
    }

    componentDidMount() {
        this.getImages();
    }

    encodeImage(element) {
        var file = element.files[0];
        var reader = new FileReader();
        reader.onloadend = () => {
            this.setState({ base64: reader.result, fileName: file.name });
        }
        reader.readAsDataURL(file);
    }

    renderUpload() {
        return (
            <div>
                <input type="file" onChange={(e) => this.encodeImage(e.target)} />
                <button onClick={() => this.upload()} className="btn btn-primary">Upload</button>
            </div>
        )
    }

    renderImages() {
        let count = 0;

        return (
            <div className="container d-block" data-bs-ride="carousel" id={this.props.puzzleId}>
                <div className="">
                    {this.state.images.map((img) => {
                        return (
                            <div className={`${count++ < 1 ? 'active' : ''}`}>
                                <img src={img.base64} className="w-100" alt={this.props.puzzleId + '-' + count} />
                            </div>
                        )
                    })}
                </div>
            </div>
        )
    }

    render() {
        let existing;
        if (this.state.images.length > 0) {
            existing = this.renderImages();
        }

        return (
            <div>
                {existing}
                {this.renderUpload()}
            </div>
        )
    }

    async upload() {
        if (this.state.base64 === '') {
            return;
        }

        const url = new URL('image/addImage', window.location.origin);

        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify({
                fileName: this.state.fileName,
                puzzleId: this.props.puzzleId,
                base64Image: this.state.base64,
                sessionId: this.props.sessionId
            })
        });

        if (response.ok) {
            const data = await response.json();
            this.setState((prevState) => {
                prevState.images.push(data);
            })
        }
    }

    async getImages() {
        let url;

        if (this.props.sessionId) {
            url = new URL('image/getSessionImages?sessionId=' + this.props.sessionId, window.location.origin);
        } else if (this.props.puzzleId) {
            url = new URL('image/getPuzzleImages?puzzleId=' + this.props.puzzleId, window.location.origin);
        } else {
            return;
        }

        const response = await fetch(url);

        if (response.ok) {
            const data = await response.json();
            this.setState({ images: data });
        }
    }
}