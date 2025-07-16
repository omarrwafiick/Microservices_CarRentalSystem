export const getOrders = async () => await fetch('https://dummyjson.com/products')
                                    .then(res => res.json()) 