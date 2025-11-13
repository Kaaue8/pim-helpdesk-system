import { Card } from "@/components/ui/card";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

interface VolumeData {
  tipo: string;
  volume: number;
}

interface DashboardBarChartProps {
  title: string;
  data: VolumeData[];
}

export default function DashboardBarChart({
  title,
  data,
}: DashboardBarChartProps) {
  return (
    <div className="bg-gray-200 p-6 rounded-2xl">
      <Card className="p-6 bg-white rounded-xl h-full">
        <h2 className="text-xl font-bold text-gray-800 mb-4">{title}</h2>
        <ResponsiveContainer width="100%" height={300}>
          <BarChart data={data}>
            <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
            <XAxis
              dataKey="tipo"
              stroke="#6b7280"
              angle={-15}
              textAnchor="end"
              height={80}
            />
            <YAxis stroke="#6b7280" />
            <Tooltip
              contentStyle={{
                backgroundColor: "#fff",
                border: "1px solid #e5e7eb",
                borderRadius: "8px",
              }}
            />
            <Bar dataKey="volume" fill="#8B5CF6" radius={[8, 8, 0, 0]} />
          </BarChart>
        </ResponsiveContainer>
      </Card>
    </div>
  );
}
