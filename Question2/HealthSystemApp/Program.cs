using System;
using System.Collections.Generic;
using System.Linq;

// Patient class
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

// Prescription class
public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}

// Generic Repository class
public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
        return false;
    }
}

// HealthSystemApp class
public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Alice Smith", 30, "Female"));
        _patientRepo.Add(new Patient(2, "John Doe", 45, "Male"));
        _patientRepo.Add(new Patient(3, "Mary Johnson", 28, "Female"));

        _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now.AddDays(-2)));
        _prescriptionRepo.Add(new Prescription(2, 1, "Amoxicillin", DateTime.Now.AddDays(-1)));
        _prescriptionRepo.Add(new Prescription(3, 2, "Ibuprofen", DateTime.Now));
        _prescriptionRepo.Add(new Prescription(4, 3, "Vitamin C", DateTime.Now.AddDays(-5)));
        _prescriptionRepo.Add(new Prescription(5, 2, "Aspirin", DateTime.Now.AddDays(-3)));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap = _prescriptionRepo.GetAll()
            .GroupBy(p => p.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("\nList of Patients:");
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
        }
    }

    public void PrintPrescriptionsForPatient(int patientId)
    {
        if (_prescriptionMap.ContainsKey(patientId))
        {
            Console.WriteLine($"\nPrescriptions for Patient ID {patientId}:");
            foreach (var prescription in _prescriptionMap[patientId])
            {
                Console.WriteLine($"- {prescription.MedicationName} (Issued on: {prescription.DateIssued.ToShortDateString()})");
            }
        }
        else
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }
    }

    public void Run()
    {
        SeedData();
        BuildPrescriptionMap();
        PrintAllPatients();

        Console.Write("\nEnter a Patient ID to view prescriptions: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            PrintPrescriptionsForPatient(id);
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }
}

// Main Method
public class Program
{
    public static void Main()
    {
        HealthSystemApp app = new HealthSystemApp();
        app.Run();
    }
}
