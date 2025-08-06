import { useForm } from "react-hook-form";
import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import AuthHeader from "../../components/shared/AuthHeader";
import Footer from "../../components/shared/Footer";
import { register as registerUser } from "../../services/api/authService";
import { useAuth } from "../../contexts/AuthContext";

export default function RegisterPage() {
  const { register, handleSubmit, watch } = useForm();
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const password = watch("password");

  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/");
    }
  }, [isAuthenticated, navigate]);

  const onSubmit = async (data) => {
    setError("");
    if (data.password !== data.confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    try {
      await registerUser({
        email: data.email,
        username: data.username,
        password: data.password,
      });
      navigate("/login");
    } catch (e) {
      setError(e.message || "Registration failed");
    }
  };

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <AuthHeader />
      <main className="flex-1 flex items-center justify-center px-4 py-12">
        <div className="bg-white p-8 rounded-2xl shadow-xl w-full max-w-md border border-gray-200 scale-[0.95]">
          <h2 className="text-3xl font-bold text-center mb-8 text-gray-900">Register</h2>
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div>
              <label className="italic text-base text-gray-700">Email</label>
              <input
                {...register("email", { required: true })}
                type="email"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1"
              />
            </div>
            <div>
              <label className="italic text-base text-gray-700">Username</label>
              <input
                {...register("username", { required: true })}
                type="text"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1"
              />
            </div>
            <div>
              <label className="italic text-base text-gray-700">Password</label>
              <input
                {...register("password", { required: true })}
                type="password"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1"
              />
            </div>
            <div>
              <label className="italic text-base text-gray-700">Confirm Password</label>
              <input
                {...register("confirmPassword", { required: true })}
                type="password"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg mt-1"
              />
            </div>
            {error && <div className="text-red-500 text-sm text-center">{error}</div>}
            <button
              type="submit"
              className="w-full cursor-pointer bg-blue-600 hover:bg-blue-700 text-white py-2 rounded-xl font-bold"
            >
              Register
            </button>
          </form>
          <div className="mt-6 text-center italic text-base text-gray-700">
            Already registered?{" "}
            <Link to="/login" className="text-blue-600 underline hover:text-blue-800">
              Log in here
            </Link>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
