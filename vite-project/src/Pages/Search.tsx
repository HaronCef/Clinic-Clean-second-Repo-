import { useAuth } from "../Context/AuthContext";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { Button } from "react-bootstrap";
import {GetDoctor} from "../Api/GetDoctor"
import { PostSearch } from "../Api/PostSearch";


const Search = () => {
const { user } = useAuth();
const navigate = useNavigate();
const [ErrorMessage, setErrorMessage] = useState<string | null>(null);
const [doctors, setDoctors] = useState<any[]>([]);
const [selectedDoctor, setSelectedDoctor] = useState<number | null>(null);

useEffect(() => {
    const fetchData = async () => {
        try {
            const data = await GetDoctor();
            setDoctors(data);
        } catch (error) {
            console.error("Failed to fetch doctors:", error);
            setErrorMessage("Failed to fetch doctor information. Please try again later.");
        }
    }
    fetchData();
}
, []);

const handleSubmit = () => {
    try {
        if (!selectedDoctor) {
            setErrorMessage("Пожалуйста, выберите врача.");
            return;
        }
        if (!user) {
            setErrorMessage("Пожалуйста, войдите в систему.");
            return;
        }
       PostSearch(selectedDoctor, user?.id);
    }catch (error) {
        console.error("Failed to submit search:", error);
        setErrorMessage("Search request failed. Please try again later.");
    }
}


return (
    <>
        <div><img data-aos="fade-down" onClick={() => navigate("/")} src="./clinic-logo.png" alt="" className="d-flex justify-content-start m-4"/></div>

            <div className="container mt-5">

            <h2 className="mb-4 text-center">Выберите врача</h2>

            {ErrorMessage && (
                <div className="alert alert-danger text-center">{ErrorMessage}</div>
            )}

            <div className="row">
                <div className="col-md-6">
                    <div className="list-group">
                        {doctors.map((doctor) => (
                            <button
                                key={doctor.id}
                                className={`list-group-item list-group-item-action ${
                                    selectedDoctor === doctor.id ? "active" : ""
                                }`}
                                onClick={() => setSelectedDoctor(doctor.id)}
                            >
                                <strong>{doctor.name}</strong> — {doctor.specialization}
                            </button>
                        ))}
                    </div>
                </div>
                <div data-aos="fade-left" className="col-md-6 d-flex justify-content-center align-items-center">
                    <Button variant="primary" onClick={handleSubmit}>
                        Отправить
                    </Button>
                </div>
            </div>
        </div>
    </>
)
}

export default Search;  