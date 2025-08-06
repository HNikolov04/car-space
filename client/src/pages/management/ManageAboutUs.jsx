import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import StaticPagesHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import { getAboutUs, updateAboutUs } from "../../services/api/aboutUsService";
import { useAuth } from "../../contexts/AuthContext";

export default function AboutEditPage() {
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuth();

  const [formData, setFormData] = useState({ title: "", message: "" });
  const [loading, setLoading] = useState(true);
  const [updatedAt, setUpdatedAt] = useState("");

  useEffect(() => {
    if (!isAuthenticated || user?.role !== "Administrator") {
      navigate("/");
      return;
    }

    getAboutUs()
      .then((data) => {
        if (!data) throw new Error("No data");
        setFormData({ title: data.title, message: data.message });
        setUpdatedAt(data.updatedAt);
      })
      .catch(() => navigate("/about"))
      .finally(() => setLoading(false));
  }, [isAuthenticated, user, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateAboutUs(formData);
      navigate(-1);
    } catch (err) {
      console.error("Failed to update About Us:", err.message);
    }
  };

  if (loading) return <div className="text-center p-10">Loading...</div>;

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col">
      <StaticPagesHeader />

      <main className="flex-1 p-6 flex justify-center">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-3xl bg-white border border-gray-300 shadow-md rounded-xl p-8"
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

          <label className="block text-sm font-medium mb-1">Message</label>
          <textarea
            name="message"
            value={formData.message}
            onChange={handleChange}
            rows={8}
            className="w-full border rounded-md px-3 py-2 mb-6"
            required
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
