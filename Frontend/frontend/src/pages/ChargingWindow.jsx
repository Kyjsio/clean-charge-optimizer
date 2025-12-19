import 'bootstrap/dist/css/bootstrap.min.css';
import { api } from '../api/CleanEnergyApi';
import React,{ useState } from 'react';

function ChargingWindow() {
    const [data, setData] = useState([]);
    const [hours, setHours] = useState('');

    const fetchChargingWindow = async (hours) => {
        try {
            const response = await api.CleanEnergy.chargingwindow(hours);
            setData(response);
            //console.log(response);
            //console.log(data);
        }
        catch (error) {
            console.error('Error fetching mix energy data:', error);
        }
    };

    const HandleSubmit = (e) => {
        e.preventDefault();
        console.log("hours", hours);
        fetchChargingWindow(hours);
    };
    return (

        <div>
            <h1>Charging Window Page</h1>
            <form onSubmit={HandleSubmit} >
                <label htmlFor="hours">Hours(1-6)</label>
               <input
                        type="number"
                        id="hours"
                        min="1"
                        max="6"
                        value={hours}
                        onChange={(e) => setHours(e.target.value)}
                    />
                <button type="submit" className="btn btn-primary">
                    Submit
                </button>
            </form>
            <div>
                {data.map((dayItem, index) => ( 
                    <div key={index} style={{ marginBottom: '50px' }}>
                        <h3>Best Charging Window for {hours} hours</h3>
                        {}
                        <h3>Start {new Date(dayItem.startTime).toUTCString()}  to {new Date(dayItem.endTime).toUTCString()}</h3>
                        <h3>Average Clean Enegry in this time:  {dayItem.averageCleanEnergyPercent}%</h3>
                        <hr />
                    </div>
                ))}
            </div>
        </div>
    );
}
export default ChargingWindow;
