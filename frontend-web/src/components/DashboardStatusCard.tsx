import { Card } from "@/components/ui/card";
import { LucideIcon } from "lucide-react";

interface DashboardStatusCardProps {
  label: string;
  value: number | string;
  icon: LucideIcon;
  colorClass: string;
}

export default function DashboardStatusCard({
  label,
  value,
  icon: Icon,
  colorClass,
}: DashboardStatusCardProps) {
  return (
    <Card
      className={`p-6 bg-gradient-to-br border-0 rounded-2xl shadow-md ${colorClass}`}
    >
      <div className="flex items-center justify-between">
        <div>
          <p className="text-gray-700 font-semibold mb-2">{label}</p>
          <p className="text-4xl font-bold">{value}</p>
        </div>
        <Icon size={40} className="opacity-70" />
      </div>
    </Card>
  );
}
