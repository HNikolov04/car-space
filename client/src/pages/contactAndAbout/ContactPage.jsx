import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import StaticPagesHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import { getContactUs } from "../../services/api/contactUsService";
import { useAuth } from "../../contexts/AuthContext";

export default function ContactUs() {
  const { isAuthenticated, user } = useAuth();
  const [data, setData] = useState(null);

  useEffect(() => {
    getContactUs()
      .then(setData)
      .catch(() => setData(null));
  }, []);

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <StaticPagesHeader />

      <main className="flex-1 p-8 max-w-4xl mx-auto text-center">
        {data ? (
          <>
            <h1 className="text-3xl font-bold mb-4">{data.title}</h1>
            <p className="text-gray-700 text-lg mb-4 whitespace-pre-line">{data.message}</p>

            <p className="text-md">
              📧 Email:{" "}
              <a href={`mailto:${data.email}`} className="text-blue-600">
                {data.email}
              </a>
            </p>
            <p className="text-md mt-2">📞 Phone: {data.phone}</p>
          </>
        ) : (
          <p className="text-gray-500">Loading contact info...</p>
        )}
      </main>

      <Footer />
    </div>
  );
}
