import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";
import path from "path";
import { defineConfig } from "vite";
import { vitePluginManusRuntime } from "vite-plugin-manus-runtime";

// Resolver compatível com Vite + TypeScript
const r = (p: string) => path.resolve(new URL(".", import.meta.url).pathname, p);

export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
    vitePluginManusRuntime()
  ],

  resolve: {
    alias: {
      "@": r("src"),
      "@shared": r("../shared"),
      "@assets": r("attached_assets"),
    },
  },

  build: {
    outDir: r("dist/public"),
    emptyOutDir: true,
  },

  server: {
    port: 3000,
    strictPort: false,
    host: true,
    allowedHosts: [
      ".manuspre.computer",
      ".manus.computer",
      ".manus-asia.computer",
      ".manuscomputer.ai",
      ".manusvm.computer",
      "localhost",
      "127.0.0.1"
    ],
    fs: {
      strict: true,
      deny: ["**/.*"],
    },
  },
});
