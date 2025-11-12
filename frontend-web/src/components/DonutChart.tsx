import { PieChart, Pie, Cell, ResponsiveContainer } from "recharts";

interface DonutChartProps {
  data: {
    aberto: number;
    andamento: number;
    resolvido: number;
    total: number;
  };
}

export default function DonutChart({ data }: DonutChartProps) {
  const chartData = [
    { name: "Em aberto", value: data.aberto, fill: "#e9d5ff" },
    { name: "Em andamento", value: data.andamento, fill: "#a855f7" },
    { name: "Resolvido", value: data.resolvido, fill: "#6b21a8" },
  ];

  return (
    <ResponsiveContainer width="100%" height={200}>
      <PieChart>
        <Pie
          data={chartData}
          cx="50%"
          cy="50%"
          innerRadius={60}
          outerRadius={100}
          paddingAngle={2}
          dataKey="value"
        >
          {chartData.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={entry.fill} />
          ))}
        </Pie>
      </PieChart>
    </ResponsiveContainer>
  );
}
