import React, { Component } from 'react';
import { TimeEntry } from './TimeEntry';

export class User extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: this.props.user,
            solvingSessionId: this.props.solvingSessionId,
            userSelection: [],
            nameSearch: ''
        };

        this.createUser = this.createUser.bind(this);
        this.setSelectedUser = this.setSelectedUser.bind(this);
    }

    renderSearchUser() {
        let userSelect;
        if (this.state.userSelection.length !== 0) {
            userSelect = (
                <ul>
                    {this.state.userSelection.map((user) => <li key={user.id} value={user.id} onClick={() => this.setSelectedUser(user)}>{user.name} </li> )}
                </ul>
            )
        } else if (this.state.nameSearch.length > 0) {
            userSelect = (
                <div>
                    <button onClick={() => this.createUser(this.state.nameSearch)}>Create User</button>
                </div>
            )
        }

        return (
            <div>
                <input type="text"
                    value={this.state.nameSearch}
                    placeholder="User name search"
                    onChange={(e) => {
                        this.setState({ nameSearch: e.target.value });
                        this.userSearch(e.target.value);
                    }} />
                <ul>
                    {userSelect}
                </ul>
            </div>
        )
    }

    renderUser() {
        return (
            <div className="card-body">
                <h4 className="card-title">{this.state.user.id} - {this.state.user.name}</h4>
                <TimeEntry sessionId={this.state.solvingSessionId} userId={this.state.user.id} completed={this.props.completed} />
            </div>
        )
    }

    render() {
        let contents;
        if (this.state.user === null || this.state.user === undefined) {
            contents = this.renderSearchUser();
        } else {
            contents = this.renderUser();
        }

        return (
            <div className="user col card">
                {contents}
            </div>
        )
    }

    setSelectedUser(user) {
        this.setState({ nameSearch: '', userSelection: [] });
        this.props.userSelect(user);
    }

    async createUser(userName) {
        const url = new URL('user/createUser?userName=' + userName, window.location.origin);
        const response = await fetch(url);

        const data = await response.json();

        this.setSelectedUser(data);
    }

    async userSearch(name) {
        const url = new URL('user/FindUsersByName?name=' + name, window.location.origin);
        const response = await fetch(url);

        if (response.status == 200) {
            const data = await response.json();
            this.setState({ userSelection: data });
        } else {
            this.setState({ userSelection: [] });
        }
    }
}