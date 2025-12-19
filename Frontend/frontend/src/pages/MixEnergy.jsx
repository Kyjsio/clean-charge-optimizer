import { useState,useEffect } from 'react'
import 'bootstrap/dist/css/bootstrap.min.css';
import { api } from '../api/CleanEnergyApi';
import { PieChart, Pie, Label,Tooltip,Cell } from 'recharts';
function MixEnergy() {
    const [data, setData] = useState([]);
    useEffect(() => {
    const fetchMixEnergyData = async () => {
        try {
            const response = await api.CleanEnergy.mixFuel();
            setData(response);
            //console.log(response);
            //console.log(data);
        }
        catch (error) {
            console.error('Error fetching mix energy data:', error);
        }
    }
        fetchMixEnergyData();
    },[]);
    const Colors = ['#03fca1', '#03a9fc', '#03ebfc', '#7ffc03', '#0384fc', '#03fcdb', '#03c6fc', '#037ffc','#0313fc'];
    return (
        <div className='container-fluid mt-5 min-vh-100'>
            <h1>Charging Window Page</h1>
            {data.map((dayItem,) => (
                <div className="row" key={dayItem.date}>
                    <div className="col-sm" style={{ marginBottom: '50px' }}>
               
                       
                            <PieChart width={500} height={500}>
                                <Pie
                                    data={dayItem.averageFuelMix}
                                    dataKey="perc"
                                    nameKey="fuel"   
                                    label={({ fuel, perc }) => `${fuel}: ${perc}%`}
                                 >
                                    {dayItem.averageFuelMix.map((entry, index) => (
                                        <Cell
                                            key={`cell-${index}`}
                                            fill={Colors[index]}
                                        />
                                    ))}
                                </Pie>
                                <Tooltip />
                             </PieChart>
                    
                    </div>
                    <div className="col-sm">
                        <h2>Data: {dayItem.date}</h2>
                        <h3>Czysta Energia: {dayItem.averageCleanEnergyPercent}%</h3>
                    </div>
             
                    
              
                </div>
            ))}
            
        </div>
    );
}

export default MixEnergy
