
import axios from 'axios';
import type { PersonForm } from '../TypeModel/Types';

export const AddDoctor = async (person: PersonForm) => {
  const response = await axios.post('https://localhost:7120/Doctors/Create', person);
  return response.data;
};