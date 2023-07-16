import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = {
            forecasts: [],
            loading: true,
            serverInfoLoading: true
        };
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static renderServerInfo(serverInfo) {
        return (
            <div><p>API server address: {serverInfo.address}</p></div>
        );
    }

    componentDidMount() {
        this.populateWeatherData();
        this.populateServerInfo();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);

        let serverInfo = this.state.serverInfoLoading
            ? <p><em>Loading server info...</em></p>
            : FetchData.renderServerInfo(this.state.serverInfo);

        return (
            <div>
                <h1 id="tabelLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {serverInfo}
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('api/weatherforecast');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }

    async populateServerInfo() {
        try {
            const response = await fetch('api/instance/current');
            const data = await response.json();
            this.setState(prevState => ({
                forecasts: prevState.forecasts,
                loading: prevState.loading,
                serverInfo: data,
                serverInfoLoading: false
            }));
        }
        catch (e) {
            console.log(e);
            this.setState(prevState => ({
                forecasts: prevState.forecasts,
                loading: prevState.loading,
                serverInfo: {address: "unavailabe"},
                serverInfoLoading: false
            }));
        }
    }
}
