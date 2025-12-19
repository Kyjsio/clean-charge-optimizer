import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

import MixEnergy from './pages/MixEnergy';
import Navbar from './components/Navbar';
import Charging from './pages/ChargingWindow';
function App() {

    return (
        <>
            <BrowserRouter>
                <Navbar />
                <Routes>
                    <Route path="/mixEnergy" element={<MixEnergy />} />
                    <Route path="/" element={<Navigate to="/mixEnergy" />} />
                    <Route path="/chargingWindow" element={<Charging />} />
                </Routes>
            </BrowserRouter>
        </>
    );
}

export default App;