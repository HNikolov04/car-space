import { Link } from "react-router-dom";
import MainPageHeader from "../components/homePageComponents/MainPageHeader";
import Footer from "../components/shared/Footer";

export default function InternalServerErrorPage() {
  return (
    <div className="min-h-screen flex flex-col">
      <MainPageHeader />
      <main className="flex flex-col items-center justify-center flex-1 bg-gray-50 px-6 text-center">
        <h1 className="text-5xl font-bold text-gray-800 mb-4">500</h1>
        <p className="text-2xl text-gray-600 mb-8">Something went wrong on our end.</p>
        <Link
          to="/"
          className="text-white bg-blue-600 hover:bg-blue-700 px-6 py-3 rounded-md text-lg font-semibold transition cursor-pointer"
        >
          Go back home
        </Link>
      </main>
      <Footer />
    </div>
  );
}
