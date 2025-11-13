import { Card } from "@/components/ui/card";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

interface ChartData {
  dia: string;
  chamados: number;
}

interface DashboardLineChartProps {
  title: string;
  data: ChartData[];
}

export default function DashboardLineChart({
  title,
  data,
}: DashboardLineChartProps) {
  return (
    <div className="bg-gray-200 p-6 rounded-2xl">
      <Card className="p-6 bg-white rounded-xl h-full">
        <h2 className="text-xl font-bold text-gray-800 mb-4">{title}</h2>
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={data}>
            <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
            <XAxis dataKey="dia" stroke="#6b7280" />
            <YAxis stroke="#6b7280" />
            <Tooltip
              contentStyle={{
                backgroundColor: "#fff",
                border: "1px solid #e5e7eb",
                borderRadius: "8px",
              }}
            />
            <Line
              type="monotone"
              dataKey="chamados"
              stroke="#8B5CF6"
              strokeWidth={3}
              dot={{ fill: "#8B5CF6", r: 4 }}
            />
          </LineChart>
        </ResponsiveContainer>
      </Card>
    </div>
  );
}
