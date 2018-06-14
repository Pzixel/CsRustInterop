//Rust code
#[no_mangle]
pub extern fn println(bytes: *const u8, count: usize) -> i32 {
    let slice = unsafe { std::slice::from_raw_parts(bytes, count) };
    let str = std::str::from_utf8(slice);
    match str {
        Ok(str) => {
            println!("{}", str);
            1
        },
        Err(_) => -1
    }
}