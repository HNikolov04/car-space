import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import StaticPagesHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import { getContactUs, updateContactUs } from "../../services/api/contactUsService";
import { useAuth } from "../../contexts/AuthContext";

export default function ContactEditPage() {
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();

  const [formData, setFormData] = useState({
    title: "",
    email: "",
    phone: "",
    address: "",
    message: "",
  });
  const [loading, setLoading] = useState(true);
  const [updatedAt, setUpdatedAt] = useState("");

  useEffect(() => {
    if (!isAuthenticated || user?.role !== "Administrator") {
      navigate("/");
      return;
    }

    getContactUs()
      .then((data) => {
        if (!data) throw new Error("No data");
        setFormData({
          title: data.title,
          email: data.email,
          phone: data.phone,
          message: data.message,
        });
        setUpdatedAt(data.updatedAt);
      })
      .catch(() => navigate("/contact"))
      .finally(() => setLoading(false));
  }, [isAuthenticated, user, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateContactUs(formData);
      navigate("/management");
    } catch (err) {
      console.error("Failed to update Contact Us:", err.message);
    }
  };

  if (loading) return <div className="text-center p-10">Loading...</div>;

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <StaticPagesHeader />

      <main className="flex-1 flex justify-center px-4 py-10">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-3xl bg-white rounded-xl shadow-lg border border-gray-300 p-8"
        >
          {updatedAt && (
            <p className="text-sm text-gray-500 mb-4">
              Last updated: {new Date(updatedAt).toLocaleDateString()}
            </p>
          )}

          <label className="block text-sm font-medium mb-1">Title</label>
          <input
            name="title"
            value={formData.title}
            onChange={handleChange}
            className="w-full border rounded-md px-3 py-2 mb-4"
            required
          />

          <label className="block text-sm font-medium mb-1">Email</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className="w-full border rounded-md px-3 py-2 mb-4"
            required
          />

          <label className="block text-sm font-medium mb-1">Phone Number</label>
          <input
            type="text"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            className="w-full border rounded-md px-3 py-2 mb-4"
          />

          <label className="block text-sm font-medium mb-1">Message</label>
          <textarea
            name="message"
            value={formData.message}
            onChange={handleChange}
            rows={6}
            className="w-full border rounded-md px-3 py-2 mb-6"
          />

          <div className="flex justify-between flex-wrap gap-4">
            <button
              type="button"
              onClick={() => navigate(-1)}
              className="px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
            >
              Save Changes
            </button>
          </div>
        </form>
      </main>

      <Footer />
    </div>
  );
}
