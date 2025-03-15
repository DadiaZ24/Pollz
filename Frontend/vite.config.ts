import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import dotenv from 'dotenv'

dotenv.config();

// https://vite.dev/config/
export default defineConfig({
  define: {
    'import.meta.env.VITE_API_URL': JSON.stringify(process.env.VITE_API_URL)
  },
  plugins: [react()],
})
