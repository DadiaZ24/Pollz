import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import HttpBackend from "i18next-http-backend"; // Optional if you're loading from a backend

i18n
  .use(HttpBackend) // Optional if you're loading translations from a server
  .use(initReactI18next)
  .init({
    lng: "pt",
    backend: {
      loadPath: "/../../congigs/lang/{{lng}}/{{ns}}.json", // Path to your translation files
    },
    interpolation: {
      escapeValue: false, // React already escapes values
    },
  });

export default i18n; // Export the i18n instance
