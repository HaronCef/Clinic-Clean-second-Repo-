import axios from "axios";
import { use, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../Context/AuthContext";
import { Button } from "react-bootstrap";
import { GetUsers } from "../Api/GetAll";
import { PromoteToManager } from "../Api/PromoteToManager";
import { DeleteDoctor } from "../Api/DeleteDoctor";
import { useForm } from "react-hook-form";
import type { PersonForm } from "../TypeModel/Types";
import { AddDoctor } from "../Api/AddDoctor";
import { DeleteUser } from "../Api/DeleteUser";
import { GetDoctor } from "../Api/GetDoctor";

const specializations = [
    "Therapist",
    "Surgeon",
    "Cardiologist",
    "Neurologist",
    "Dermatologist",
    "Oncologist",
    "Ophthalmologist",
    "Psychiatrist",
    "Dentist"
];

const AdminPanel = () => {

  const { register, handleSubmit, watch, reset } = useForm<PersonForm>();
  const isDoctor = watch("isDoctor");
const { user } = useAuth();
const [users, setUsers] = useState<any[]>([]);
const [doctor, setDoctor] = useState<any[]>([]);
const [ErrorMessage, setErrorMessage] = useState<string | null>(null);
const navigate = useNavigate();
const [selectedUser, setSelectedUser] = useState<number | null>(null);
const [selectedDoctor, setSelectedDoctor] = useState<number | null>(null);
useEffect(() => {
    fetchData();
    fetchDoctors();
}, []);

const fetchData = async () => {
        try {
            const data = await GetUsers();
            setUsers(data);
        } catch (error) {
            console.error("Failed to fetch doctors:", error);
            setErrorMessage("Failed to fetch doctor information. Please try again later.");
        }
    }

const fetchDoctors = async () => {
  try {
    const data = await GetDoctor();
    setDoctor(data);
  } catch (error) {
    console.error("Failed to fetch doctors:", error);
    setErrorMessage("Failed to fetch doctor information. Please try again later.");
  }
};

const handlePromote = async () => {
    try {
        if (!selectedUser) {
            setErrorMessage("Пожалуйста, выберите врача.");
            return;
        }
        if (!user) {
            setErrorMessage("Пожалуйста, войдите в систему.");
            return;
        }
      await PromoteToManager(selectedUser)
       
    } catch (error) {
        console.error("Failed to submit search:", error);
    }
    }

const handleDelete = async () => {
    try {
        if (!selectedUser) {
            setErrorMessage("Пожалуйста, выберите врача.");
            return;
        }
        if (!user) {
            setErrorMessage("Пожалуйста, войдите в систему.");
            return;
        }
        await DeleteUser(selectedUser)
        await fetchData();
        setSelectedUser(null);
    } catch (error) {
        console.error("Failed to submit search:", error);
    }
 }

 const onSubmit = async (data: PersonForm) => {
    try {
    await AddDoctor(data);
    alert("Додано!");
    } catch (error) {
        console.error("Failed to submit form:", error);
        setErrorMessage("Failed to submit form. Please try again later.");
    }
 }

 const handleDeleteDoctor = async () => {
    try { 
        if (!selectedDoctor) {
            setErrorMessage("Пожалуйста, выберите врача.");
            return;
        }
        if (!user) {
            setErrorMessage("Пожалуйста, войдите в систему.");
            return;
        }
        await DeleteDoctor(selectedDoctor);
        await fetchDoctors();
        setSelectedDoctor(null);
    } catch (error) {
        console.error("Failed to delete doctor:", error);
        setErrorMessage("Failed to delete doctor. Please try again later.");
    }
  }


return(
 <>
 <div><img data-aos="zoom-in" onClick={() => navigate("/")} src="./clinic-logo.png" alt="" className="d-flex justify-content-start m-4"/></div>
 <div className="container d-flex justify-content-center align-items-center mt-5">
  <form data-aos="fade-up"
    onSubmit={handleSubmit(onSubmit)} 
    className="bg-light p-4 rounded shadow-sm w-100" 
    style={{ maxWidth: 500 }}
  >
    <h4 className="text-center mb-4">Додавання користувача</h4>

    <div className="mb-3">
      <label htmlFor="name" className="form-label">Ім’я:</label>
      <input 
        type="text" 
        id="name" 
        className="form-control" 
        {...register("name", { required: true })} 
      />
    </div>


      <div className="mb-3">
        <label htmlFor="specialization" className="form-label">Спеціалізація:</label>
        <select 
          className="form-select" 
          id="specialization" 
          {...register("specialization", { required: isDoctor })} 
        >
          <option value="">Оберіть спеціалізацію</option>
          {specializations.map((spec) => (
            <option key={spec} value={spec}>
              {spec}
            </option>
          ))}
        </select>
      </div>

    <div className="d-grid">
      <button type="submit" className="btn btn-primary">
        Додати
      </button>
    </div>
  </form>
</div>

          {user?.role === "admin" &&( <div className="container mt-5">

            <h2 className="mb-4 text-center">Выберите Пользователя</h2>

            {ErrorMessage && (
                <div className="alert alert-danger text-center">{ErrorMessage}</div>
            )}

            <div className="row">
                <div className="col-md-6">
                    <div className="list-group">
                        {users.map((user) => (
                            <button 
                                key={user.id}
                                className={`list-group-item list-group-item-action ${
                                    selectedUser === user.id ? "active" : ""
                                }`}
                                onClick={() => setSelectedUser(user.id)}
                            >
                                <strong>{user.name}</strong> — {user.specialization}
                            </button>
                        ))}
                    </div>
                </div>
                <div className="col-md-6 d-flex justify-content-center align-items-center gap-3 p-3">
                    <Button data-aos="fade-left" variant="primary" onClick={handlePromote}>
                        Менеджер
                    </Button>
                    <Button data-aos="fade-left" variant="danger" onClick={handleDelete}>
                        Удалить
                    </Button>
                </div>
            </div>
        </div>)}

        <div className="container mt-5">

            <h2 className="mb-4 text-center">Выберите Доктора</h2>

            

            <div className="row">
                <div className="col-md-6">
                    <div className="list-group">
                        {doctor.map((doctor) => (
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
                <div className="col-md-6 d-flex justify-content-center align-items-center gap-3 p-3">
                    <Button data-aos="fade-left" variant="danger" onClick={handleDeleteDoctor}>
                        Удалить
                    </Button>
                </div>
            </div>
        </div>
    </>
)
}

export default AdminPanel;