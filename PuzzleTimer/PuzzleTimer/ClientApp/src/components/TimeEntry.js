import React, { Component } from 'react';
import { DateTime } from 'luxon';
import eventBus from './EventBus';

export class TimeEntry extends Component {
    constructor(props) {
        super(props);
        this.state = {
            timeEntry: {},
            running: false,
            elapsed: null,
            intervalId: 0,
            start: DateTime.now(),
            total: '',
            enabled: true
        }

        this.startTimer = this.startTimer.bind(this);
        this.stopTimer = this.stopTimer.bind(this);
        this.start  = this.start.bind(this);
        this.tick = this.tick.bind(this);
        this.stop = this.stop.bind(this);
        this.handleTimerEvent = this.handleTimerEvent.bind(this);
    }

    componentDidMount() {
        this.getCurrent();
        this.getTotal();
        if (!this.props.completed) {
            eventBus.on("timerEvent", this.handleTimerEvent);
        }
    }

    componentWillUnmount() {
        eventBus.remove("timerEvent", this.handleTimerEvent);
        clearInterval(this.state.intervalId);
    }

    handleTimerEvent(data) {
        if (data.running) {
            this.startTimer();
        } else {
            this.stopTimer();
        }
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
                <button onClick={() => this.stopTimer()} className="btn btn-primary btn-lg">⏹︎</button>
            </div>
        )
    }

    renderStopped() {
        return (
            <div>
                <button onClick={() => this.startTimer()} className="btn btn-primary btn-lg">⏵︎</button>
            </div>
        )
    }

    render() {
        let contents = this.state.running
            ? this.renderRunning()
            : this.renderStopped();

        if ((!this.state.enabled && !this.state.running) || this.props.completed) {
            contents = null;
        }

        let enableUserCheckbox;
        if (!this.props.completed) {
            enableUserCheckbox = (<div>
                <input type="checkbox"
                    value={this.state.enabled}
                    name="checkbox"
                    onClick={() => this.setState((prevState) => { return { enabled: !prevState.enabled } })} />
                <label htmlFor="checkbox">Disable User?</label>
            </div>);
        }

        return (
            <div className="timeEntry">
                <p className="card-subtitle text-muted">{this.state.total}</p>
                {contents}
                {enableUserCheckbox}
            </div>
        )
    }

    async startTimer() {
        if (this.state.running || !this.state.enabled) {
            return;
        }

        const url = new URL(`timeEntry/Start?sessionId=${this.props.sessionId}&userId=${this.props.userId}`, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();

        this.setState({ timeEntry: data, running: true });

        this.start();
    }

    async stopTimer() {
        if (!this.state.running) {
            return;
        }

        const url = new URL('timeEntry/Stop?timeEntryId=' + this.state.timeEntry.id, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();

        this.setState({ timeEntry: data, running: false });

        this.stop();

        this.getTotal();
    }

    async getCurrent() {
        const url = new URL(`timeEntry/GetCurrent?sessionId=${this.props.sessionId}&userId=${this.props.userId}`, window.location.origin);
        const response = await fetch(url);

        if (response.status === 200) {
            const data = await response.json();

            let running = false;
            if (data.startTime && data.endTime === null) {
                running = true;
            }

            this.setState({ timeEntry: data, running: running });

            this.start();
        }
    }

    async getTotal() {
        const url = new URL(`timeEntry/GetTotalTime?sessionId=${this.props.sessionId}&userId=${this.props.userId}`, window.location.origin);
        const response = await fetch(url);

        const data = await response.text();

        this.setState({ total: data });
    }
}