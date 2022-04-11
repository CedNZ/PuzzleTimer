import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { SolvingSession } from './components/SolvingSession';
import { Puzzle } from './components/Puzzle';

import './custom.css'

export default class App extends Component {
    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/solving-session' component={SolvingSession} />
                <Route path='/puzzle' component={Puzzle} />
            </Layout>
        );
    }
}
