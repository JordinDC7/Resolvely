import * as Yup from "yup";


    const appointmentSchema = Yup.object().shape({
        appointmentTypeId: Yup.number().required("Is Required"),        
        notes: Yup.string().required("Is Required"),
        location: Yup.string().required("Is Required"),
        isConfirmed: Yup.boolean().required("Is Required"),
        appointmentStart: Yup.date().required("Is Required"),
        appointmentEnd: Yup.date().required("Is Required"),
        statusId: Yup.number().required("Is Required")
    });


    export default appointmentSchema;