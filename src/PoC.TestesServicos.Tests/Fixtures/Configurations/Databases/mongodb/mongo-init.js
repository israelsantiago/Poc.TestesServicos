
db.auth('mongodbuser', 'password');

db.createUser({
    user: 'mongodbuser',
    pwd: 'password',
    roles: [
        {
            role: 'dbOwner',
            db: 'mogodbtest',
        },
    ],
});

let res = [
    db.container.drop(),
    db.container.createIndex({ myfield: 1 }, { unique: true }),
    db.container.createIndex({ thatfield: 1 }),
    db.container.insert({ myfield: 'hello', thatfield: 'testing' }),
    db.container.insert({ myfield: 'hello2', thatfield: 'testing' }),
    db.container.insert({ myfield: 'hello3', thatfield: 'testing' }),
    db.container.insert({ myfield: 'hello4', thatfield: 'testing' }),
]

printjson(res)

/*
let error = true

if (error) {
    print('Error, exiting')
    quit(1)
}
*/
