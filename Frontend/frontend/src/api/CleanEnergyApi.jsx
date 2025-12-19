import axios from 'axios';
//const UrlBase = 'http://localhost:5008/api';
const UrlBase = '/api';

export const api =
{
    CleanEnergy:
    {
        mixFuel: async ()=> {

        try {
            const response = await axios.get(`${UrlBase}/CleanEnergy/mixFuel`);
            return response.data;
        } catch(error) {
            console.error('Error fetching clean energy data:', error);
            throw error;
        }
    },
        chargingwindow: async (ChargingTime) => {
            try {
                const response = await axios.get(`${UrlBase}/CleanEnergy/chargingwindow/${ChargingTime}`);
                return response.data;
            } catch (error) {
                console.error('Error fetching charging window data:', error);
                throw error;
            }
        }

}
};