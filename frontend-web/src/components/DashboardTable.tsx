import { Card } from "@/components/ui/card";

interface TableData {
  tecnico: string;
  tickets: number;
  sla: string;
  avaliacao: string;
}

interface DashboardTableProps {
  title: string;
  data: TableData[];
}

export default function DashboardTable({ title, data }: DashboardTableProps) {
  return (
    <div className="bg-gray-200 p-6 rounded-2xl">
      <Card className="p-6 bg-white rounded-xl h-full">
        <h2 className="text-xl font-bold text-gray-800 mb-4">{title}</h2>
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b-2 border-gray-300 bg-gray-100">
                <th className="text-left py-3 px-3 text-gray-700 font-semibold">
                  Técnico
                </th>
                <th className="text-left py-3 px-3 text-gray-700 font-semibold">
                  Tickets Totais
                </th>
                <th className="text-left py-3 px-3 text-gray-700 font-semibold">
                  SLA
                </th>
                <th className="text-left py-3 px-3 text-gray-700 font-semibold">
                  Avaliação
                </th>
              </tr>
            </thead>
            <tbody>
              {data.map((item, index) => (
                <tr
                  key={index}
                  className={index % 2 === 0 ? "bg-gray-50" : "bg-white"}
                >
                  <td className="py-3 px-3 text-gray-700">{item.tecnico}</td>
                  <td className="py-3 px-3 text-gray-700">{item.tickets}</td>
                  <td className="py-3 px-3 text-gray-700">{item.sla}</td>
                  <td className="py-3 px-3 text-gray-700">{item.avaliacao}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </Card>
    </div>
  );
}
