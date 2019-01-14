interface IBasicDataProduct {
    productId: number;
    name: string;
    desc: string;
    url: string;
}

interface ICreationViewModel {
    name: string;
    desc: string;
}

interface IProductNameGettingViewModel {
    name: string;
}

interface IUpdateViewModel {
    productId: number;
    name: string;
    desc: string;
}

export {
    IBasicDataProduct,
    ICreationViewModel,
    IProductNameGettingViewModel,
    IUpdateViewModel,
};
