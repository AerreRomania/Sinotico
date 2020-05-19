using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using  System.Data;

namespace wcfcleaners
    {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
        {
        /// <summary>
        /// Gets alarmed data.
        /// </summary>
        /// <returns>data table</returns>
        [OperationContract]
        DataTable GetAlmTable();
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>data table</returns>
        [OperationContract]
        DataTable GetGeneralTable();
        /// <summary>
        /// Gets the data using data contract.
        /// </summary>
        /// <param name="composite">The composite.</param>
        /// <returns></returns>
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        // TODO: Add your service operations here
        [OperationContract]
        string GetOperatorName(string code);
        /// <summary>
        /// Gets the progressive number.
        /// </summary>
        /// <param name="evDate">The ev date.</param>
        /// <param name="cShift">The c shift.</param>
        /// <param name="mac">The mac.</param>
        /// <returns></returns>
        [OperationContract]
        int GetProgressiveNumber(DateTime evDate, string cShift, int mac, string operatorx, string type);
        /// <summary>
        /// Inserts the new operation.
        /// </summary>
        /// <param name="evDate">The ev date.</param>
        /// <param name="cShift">The c shift.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="cStart">The c start.</param>
        /// <param name="cEnd">The c end.</param>
        /// <param name="machine">The machine.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="cType">Type of the c.</param>
        /// <param name="note">The note.</param>
        /// <param name="dateLoad">The date load.</param>
        /// <param name="progNum">The prog number.</param>
        /// <param name="tempoMin">The tempo minimum.</param>
        /// <param name="ptPrec">The pt prec.</param>
        [OperationContract]
        void InsertNewOperation(DateTime evDate, string cShift, string operatorName, DateTime cStart, DateTime cEnd,
            int machine, string reason, string cType, string note, DateTime dateLoad, int progNum, int tempoMin, string ptPrec);
        /// <summary>
        /// Updates the operation.
        /// </summary>
        /// <param name="evDate">The ev date.</param>
        /// <param name="cShift">The c shift.</param>
        /// <param name="progNum">The prog number.</param>
        [OperationContract]
        void UpdateOperation(DateTime endclean, long id, string note);

            [OperationContract]
            void UpdateOperationManually(int id, string note);

            [OperationContract]
            long GetJobId();
           
        /// <summary>
        /// Gets the shift.
        /// </summary>
        [OperationContract]
        string GetShift();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract] bool HasError();
        /// <summary>
        /// Activates alarm status
        /// </summary>
        /// <param name="id">Unique job id</param>
        [OperationContract]
        void ActivateAlarm(long id);
        /// <summary>
        /// Activates alarm status
        /// </summary>
        /// <param name="id">Unique job id</param>
        /// <param name="note">Overload argumet</param>
        [OperationContract]
        void ActivateAlarm(long id, string note);
        /// <summary>
        /// Deactivates alarm status
        /// </summary>
        /// <param name="id">Unique job id</param>
        [OperationContract]
        void DeactivateAlarm(long id);
        }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
        {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
            {
            get { return boolValue; }
            set { boolValue = value; }
            }

        [DataMember]
        public string StringValue
            {
            get { return stringValue; }
            set { stringValue = value; }
            }
        }
    }
