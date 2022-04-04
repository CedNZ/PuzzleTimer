import React, { Component } from 'react';
import { DateTime } from 'luxon'

export class TimeEntry extends Component {
    static displayName = TimeEntry.name;

    constructor(props) {
        super(props);
        this.state = {
            timeEntry: {},
            running: false,
            elapsed: null,
            intervalId: 0,
            start: DateTime.now(),
            total: ''
        }

        this.startTimer = this.startTimer.bind(this);
        this.stopTimer = this.stopTimer.bind(this);
        this.start  = this.start.bind(this);
        this.tick = this.tick.bind(this);
        this.stop = this.stop.bind(this);
    }

    componentDidMount() {
        this.getCurrent();
        this.getTotal();
    }

    start() {
        const intervalId = setInterval(() => this.tick(), 1000);
        const start = DateTime.fromISO(this.state.timeEntry.startTime);
        this.setState({ intervalId: intervalId, start: start });
    }

    tick() {
        let now = DateTime.now();

        this.setState(prevState => {
            return { elapsed: now.diff(prevState.start, ["hours", "minutes", "seconds"]).toObject() };
        });
    }

    stop() {
        clearInterval(this.state.intervalId);
    }

    renderRunning() {
        let elapsed = this.state.elapsed;

        if (elapsed == null) {
            elapsed = { hours: 0, minutes: 0, seconds: 0 };
        }

        return (
            <div>
                <p>{elapsed.hours}h {elapsed.minutes}m {elapsed.seconds.toFixed()}s</p>
                <button onClick={() => this.stopTimer()} className="btn btn-primary">■</button>
            </div>
        )
    }

    renderStopped() {
        return (
            <div>
                <button onClick={() => this.startTimer()} className="btn btn-primary">▸</button>
            </div>
        )
    }

    render() {
        let contents = this.state.running
            ? this.renderRunning()
            : this.renderStopped();

        return (
            <div className="timeEntry">
                <p className="card-subtitle text-muted">{this.state.total}</p>
                {contents}
            </div>
        )
    }

    async startTimer() {
        const response = await fetch(`timeEntry/Start?sessionId=${this.props.sessionId}&userId=${this.props.userId}`);

        const data = await response.json();

        this.setState({ timeEntry: data, running: true });

        this.start();
    }

    async stopTimer() {
        const response = await fetch('timeEntry/Stop?timeEntryId=' + this.state.timeEntry.id);

        const data = await response.json();

        this.setState({ timeEntry: data, running: false });

        this.stop();

        this.getTotal();
    }

    async getCurrent() {
        const response = await fetch(`timeEntry/GetCurrent?sessionId=${this.props.sessionId}&userId=${this.props.userId}`);

        if (response.status == 200) {
            const data = await response.json();

            this.setState({ timeEntry: data, running: true });

            this.start();
        }
    }

    async getTotal() {
        const response = await fetch(`timeEntry/GetTotalTime?sessionId=${this.props.sessionId}&userId=${this.props.userId}`);

        const data = await response.text();

        this.setState({ total: data });
    }
}