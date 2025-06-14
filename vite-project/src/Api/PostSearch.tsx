import axios from "axios";

export const PostSearch = async (doctorID : any, userID: string) => {
const payload = {
    userID,
    doctorID,
};
try {
const response = await axios.post(`https://localhost:7120/Appointments/Create`, payload);
    const data = response.data;
    return data;
}catch (error) {
    console.error("Failed to post search:", error);
    throw new Error("Search request failed. Please try again later.");
}
};