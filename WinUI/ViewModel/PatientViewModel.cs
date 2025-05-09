﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Service;

namespace WinUI.ViewModel
{
    public class PatientViewModel : IPatientViewModel
    {
        private readonly IPatientService _patient_service;
        public event PropertyChangedEventHandler? PropertyChanged;
        public string password { get; set; } = string.Empty;

        public PatientJointModel originalPatient { get; private set; }

        public PatientViewModel(IPatientService patient_service, int user_id)
        {
            _patient_service = patient_service;
            _user_id = user_id;
            originalPatient = PatientJointModel.Default;
            _ = loadPatientInfoByUserIdAsync(_user_id);
        }

        private int _user_id;
        public int userId
        {
            get => this._user_id;
            set { if (this._user_id != value) { this._user_id = value; OnPropertyChanged(); } }
        }

        // These properties stay for binding/display
        private string _name = string.Empty;
        public string name
        {
            get => this._name;
            set { if (this._name != value) { this._name = value; OnPropertyChanged(); } }
        }

        private string _email = string.Empty;
        public string email
        {
            get => this._email;
            set { if (this._email != value) { this._email = value; OnPropertyChanged(); } }
        }
        private string _username = string.Empty;
        public string username
        {
            get => this._username;
            set { if (this._username != value) { this._username = value; OnPropertyChanged(); } }
        }

        private string _address = string.Empty;
        public string address
        {
            get => this._address;
            set { if (this._address != value) { this._address = value; OnPropertyChanged(); } }
        }
        private string _phone_number = string.Empty;
        public string phoneNumber
        {
            get => this._phone_number;
            set { if (this._phone_number != value) { this._phone_number = value; OnPropertyChanged(); } }
        }
        private string _blood_type = string.Empty;
        public string bloodType
        {
            get => this._blood_type;
            set { if (this._blood_type != value) { this._blood_type = value; OnPropertyChanged(); } }
        }
        private string _allergies = string.Empty;
        public string allergies
        {
            get => this._allergies;
            set { if (this._allergies != value) { this._allergies = value; OnPropertyChanged(); } }
        }
        private DateOnly _birth_date;
        public DateOnly birthDate
        {
            get => this._birth_date;
            set { if (this._birth_date != value) { this._birth_date = value; OnPropertyChanged(); } }
        }
        private string _cnp = string.Empty;
        public string cnp
        {
            get => this._cnp;
            set { if (this._cnp != value) { this._cnp = value; OnPropertyChanged(); } }
        }
        private DateTime _registration_date;
        public DateTime registrationDate
        {
            get => this._registration_date;
            set { if (this._registration_date != value) { this._registration_date = value; OnPropertyChanged(); } }
        }

        private string _emergencyContact = string.Empty;
        public string emergencyContact
        {
            get => this._emergencyContact;
            set { if (this._emergencyContact != value) { this._emergencyContact = value; OnPropertyChanged(); } }
        }

        private double _weight;
        public double weight
        {
            get => this._weight;
            set { if (this._weight != value) { this._weight = value; OnPropertyChanged(); } }
        }

        private int _height;
        public int height
        {
            get => this._height;
            set { if (this._height != value) { this._height = value; OnPropertyChanged(); } }
        }

        private bool _is_loading;
        public bool isLoading
        {
            get => this._is_loading;
            set { if (this._is_loading != value) { this._is_loading = value; OnPropertyChanged(); } }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> loadPatientInfoByUserIdAsync(int user_id)
        {
            try
            {
                this.isLoading = true;

                bool success = await this._patient_service.loadPatientInfoByUserId(user_id);
                PatientJointModel? patient = this._patient_service.patientInfo;

                if (success && patient != PatientJointModel.Default)
                {
                    this.name = patient.patientName;
                    this.email = patient.email;
                    this.username = patient.username;
                    this.address = patient.address;
                    this.phoneNumber = patient.phoneNumber;
                    this.emergencyContact = patient.emergencyContact;
                    this.bloodType = patient.bloodType;
                    this.allergies = patient.allergies;
                    this.birthDate = patient.birthDate;
                    this.cnp = patient.cnp;
                    this.registrationDate = patient.registrationDate;
                    this.weight = patient.weight;
                    this.height = patient.height;

                    this.originalPatient = new PatientJointModel(
                        this._user_id,
                        patient.patientId,
                        this.name,
                        this.bloodType,
                        this.emergencyContact,
                        this.allergies,
                        this.weight,
                        this.height,
                        this.username,
                        "", // password unused now
                        this.email,
                        patient.birthDate,
                        this.cnp,
                        this.address,
                        this.phoneNumber,
                        this.registrationDate
                    );
                }

                this.isLoading = false;
                return success;
            }
            catch (Exception exception)
            {
                this.isLoading = false;
                Console.WriteLine($"Error loading patient info: {exception.Message}");
                return false;
            }
        }

        public async Task<bool> updateEmergencyContact(string emergencyContact)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateEmergencyContact(userId, emergencyContact);
                if (updated)
                {
                    this.emergencyContact = emergencyContact;
                    this.originalPatient.emergencyContact = emergencyContact;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateWeight(double weight)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateWeight(userId, weight);
                if (updated)
                {
                    this.weight = this.weight;
                    this.originalPatient.weight = this.weight;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateHeight(int height)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateHeight(userId, height);
                if (updated)
                {
                    this.height = height;
                    this.originalPatient.height = height;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updatePassword(string password)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updatePassword(userId, password);
                if (updated)
                {
                    this.password = password;
                    this.originalPatient.password = password;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateName(string name)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateName(userId, name);
                if (updated)
                {
                    this.name = name;
                    this.originalPatient.patientName = name;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updateAddress(string address)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updateAddress(userId, address);
                if (updated)
                {
                    this.address = address;
                    this.originalPatient.address = address;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> updatePhoneNumber(string phone_number)
        {
            try
            {
                this.isLoading = true;
                bool updated = await this._patient_service.updatePhoneNumber(userId, phone_number);
                if (updated)
                {
                    this.phoneNumber = phone_number;
                    this.originalPatient.phoneNumber = phone_number;
                }
                return updated;
            }
            finally { this.isLoading = false; }
        }

        public async Task<bool> logUpdate(int user_id, ActionType action)
        {
            return await this._patient_service.logUpdate(user_id, action);
        }
    }
}
