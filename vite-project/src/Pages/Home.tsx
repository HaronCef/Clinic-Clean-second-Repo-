"use client"
import React, { use } from "react";
import "../Styles/Home.scss";
import { useAuth } from "../Context/AuthContext";
import { useNavigate } from "react-router-dom";
import AOS from "aos";
import "aos/dist/aos.css";
import { useEffect } from "react";


const Home = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();


useEffect(() => {
    AOS.init({
      duration: 1000,
      once: true,
    });
  }, []);

  return (
    <div className="page-container">
      <div data-aos="zoom-in" className="home-header">
        <img src="/clinic-logo.png" alt="Clinic Logo" className="logo-image" />
        <div className="buttons">
        <div className="d-flex justify-content-center align-items-center">
          {user?.role}
        </div>
      { (user?.role === "admin" || user?.role === "manager") &&  ( <button onClick={() => navigate("/admin")} className="btn btn-danger p-1">Admin</button>)}
      {user === null && ( <><button data-aos="fade-down" onClick={() => navigate("/login")} className="btn btn-primary p-1">   Авторизация
    </button>
    <button data-aos="fade-down" onClick={() => navigate("/register")} className="btn btn-secondary p-2">
      Регистрация
    </button>
  </>
)}
          {user !== null && (<button data-aos="fade-down"  onClick={() => logout()}   className="btn btn-secondary p-2">Вийти</button> )}
        </div>
      </div>

      <hr className="divider" />

   {user?.role  && <div className="search-section">
        <button onClick={() => navigate("/search")} className="search-button">Подобрать лучшего врача</button>
      </div>}

      <div data-aos="zoom-in" className="doctor-cards">
        <div className="doctor-card">
          <img
            src="/doctor1-photo.jpg"
            alt="Doctor 1"
            className="doctor-photo"
          />
          <h3>Богдан Гаченко</h3>
          <p>
            Учредитель клиники и ведущий мировой специалист в сфере
            Нейрохирургии и Нейропсихологии. Проводит онлайн
            консультации по программам "Нейропсихология" и "Нейрохирургия"
          </p>
        </div>
        <div data-aos="zoom-in" className="doctor-card">
          <img
            src="/doctor2-photo.jpg"
            alt="Doctor 2"
            className="doctor-photo"
          />
          <h3>Виталий Цаль</h3>
          <p>
            Ведущий психолог клиники Дяди Богдана, проводящий онлайн
            консультации по програмам "Духовное Здоровье" и "Сдерживание Гнева
          </p>
        </div>
        <div data-aos="zoom-in" className="doctor-card">
          <img
            src="/Gvintik.jpg"
            alt="Doctor 3"
            className="doctor-photo"
          />
          <h3>Илья Винтик</h3>
          <p>Ведущий специалист Киева по физиотерапии. По совместительству ведущий интернетуры в клинике и член ассоциации "Мирового съезда врачей"</p>
        </div>
      </div>
    </div>
  );
};

export default Home;