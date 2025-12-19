
import React from 'react';
import { Link } from 'react-router-dom';

export default function Navbar() {
    return (
        <nav className="navbar navbar-expand-lg bg-justify-content-end-tertiary ">
            <div className="container-fluid">
                    <ul className="navbar-nav">
                        <li className="nav-item">
                            <Link className="nav-link" to="/mixEnergy">MixEnergy</Link>
                        </li>

                        <li className="nav-item">
                            <Link className="nav-link" to="/chargingWindow">ChargingWindow</Link>
                        </li>
                  </ul>
              </div>
        </nav>
    );
};