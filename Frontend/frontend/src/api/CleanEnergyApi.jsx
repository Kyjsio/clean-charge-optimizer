import axios from 'axios';
import { PATHS } from '../constants/apiEndpoints';

const BASE_URL = import.meta.env.VITE_API_URL;

export const api = {
    CleanEnergy: {
        mixFuel: async () => {
            try {
                const response = await axios.get(`${BASE_URL}/${PATHS.CLEAN_ENERGY}/${PATHS.MIX_FUEL}`);
                return response.data;
            } catch (error) {
                console.error('Error fetching mix fuel data:', error);
                throw error;
            }
        },

        chargingwindow: async (chargingTime) => {
            try {
                const response = await axios.get(
                    `${BASE_URL}/${PATHS.CLEAN_ENERGY}/${PATHS.CHARGING_WINDOW}/${chargingTime}`
                );
                return response.data;
            } catch (error) {
                console.error('Error fetching charging window data:', error);
                throw error;
            }
        }
    }
};