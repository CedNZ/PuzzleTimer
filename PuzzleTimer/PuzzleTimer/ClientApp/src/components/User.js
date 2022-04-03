import React, { Component } from 'react';

export class User extends Component {
    static displayName = User.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
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
        if (this.state.userSelection.length != 0) {
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
            <div>
                <h4>{this.state.user.id} - {this.state.user.name}</h4>
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
            <div>
                {contents}
            </div>
        )
    }

    setSelectedUser(user) {
        this.setState({ nameSearch: '', userSelection: [] });
        this.props.userSelect(user);
    }

    async createUser(userName) {
        const response = await fetch('user/createUser?userName=' + userName);

        const data = response.json();

        this.setState({ loading: false, user: data });
    }

    async userSearch(name) {
        const response = await fetch('user/FindUsersByName?name=' + name);

        const data = await response.json();

        this.setState({ userSelection: data });
    }
}