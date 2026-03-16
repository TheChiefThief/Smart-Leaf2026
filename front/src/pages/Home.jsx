import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { HomeCard, PageHead } from '../components/';
import { JardinImage, CalendarioImage, BiblitecaImage, CamaraImage } from "../assets/";
import Cookies from 'js-cookie';
import  "../models/App.css"

class Home extends Component {
    render() {
        return (
            <div>
                <div className="app">
                    <PageHead />
                    <main className="main">
                        <h2 className='home-title'>¡Bienvenido!</h2>
                        <HomeCard 
                            nombre="Tus plantas" descripcion="Ver todas las plantas que tienes en tu jardín." link="/huerta" src1={JardinImage} />
                        <HomeCard 
                            nombre="Calendario de riego" descripcion="Saber cuándo es el mejor momento para regar tus plantas." link="/calendario" src1={CalendarioImage}/>
                        <HomeCard 
                            nombre="Análisis de plantas" descripcion="Obtener consejos personalizados para cada planta." link="/camara" src1={CamaraImage}/>
                        <HomeCard 
                            nombre="Biblioteca botánica" descripcion="Aprender sobre plantas y descubrir nuevas especies." link="/biblioteca" src1={BiblitecaImage}/>
                      
                    </main>
                    <footer className="footer">
                        <Link to="/consultor"><button className='home-card-button'>💬 Hablar con asistente</button></Link>
                    </footer>
                </div>

            </div>
        );
    }
}
 export default Home;

    