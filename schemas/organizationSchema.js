import * as Yup from 'yup';

const organizationSchema = Yup.object().shape({
    organizationTypeId: Yup.number().required(),
    name: Yup.string().required().min(2).max(200),
    headLine: Yup.string().min(2).max(200),
    description: Yup.string().min(2).max(10000),
    logo: Yup.string().min(2).max(255),
    locationId: Yup.number().required(),
    phone: Yup.string().min(2,).max(50),
    siteUrl: Yup.string().min(2).max(255)
})

export default organizationSchema;