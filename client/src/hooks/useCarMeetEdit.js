import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  getCarMeetById,
  updateCarMeetListing,
} from "../services/api/carMeetListingService";

export default function useCarMeetEdit() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [formData, setFormData] = useState(null);
  const [newImageFile, setNewImageFile] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchMeet = async () => {
      try {
        const data = await getCarMeetById(id);
        setFormData({
          id: data.id,
          title: data.title,
          description: data.description,
          city: data.city,
          address: data.address,
          meetDate: data.meetDate.split("T")[0],
          imageUrl: data.imageUrl,
          updatedAt: data.updatedAt,
        });
      } catch {
        navigate("/car-meet");
      } finally {
        setLoading(false);
      }
    };

    fetchMeet();
  }, [id, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e) => {
    setNewImageFile(e.target.files[0]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const payload = {
      ...formData,
      imageFile: newImageFile || null,
    };

    try {
      await updateCarMeetListing(payload);
      navigate(`/car-meet/${id}`);
    } catch (err) {
      console.error("Failed to update car meet:", err.message);
    }
  };

  const handleCancel = () => {
    navigate(-1);
  };

  return {
    formData,
    newImageFile,
    loading,
    handleChange,
    handleImageChange,
    handleSubmit,
    handleCancel,
  };
}
