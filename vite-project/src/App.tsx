import { useState } from 'react'
import './App.css'
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom'
import { useAuth } from './Context/AuthContext'
import Home from './Pages/Home'
import Registration from './Pages/Registration'
import AdminPanel from './Pages/AdminPanel'
import Authentication from './Pages/Authentication'
import Search from './Pages/Search'


function App() {
  const [count, setCount] = useState(0)
  const {user} = useAuth();

  return (
    <>
<Router>
  <Routes>
    <Route path="/" element={<Home/>} /> 
   <Route path="/register" element={<Registration/>} />
    <Route path="/admin" element={user?.role === "admin" || "manager" ? <AdminPanel/> : <Navigate to="/"/>} />
    <Route path="/login" element={<Authentication/>} />
    <Route path="/search" element={user?.role != undefined ? <Search/> : <Navigate to="/"/>}/>
  </Routes>
</Router>
    </>

  )
}

export default App

