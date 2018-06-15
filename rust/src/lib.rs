#[repr(C)]
pub struct StringRef {
    bytes: *const u8,
    length: usize
}

#[no_mangle]
pub extern fn println(array: *const StringRef, count: usize) -> i32 {
    let array_slice = unsafe { std::slice::from_raw_parts(array, count) };
    for str in array_slice {
        let slice = unsafe { std::slice::from_raw_parts(str.bytes, str.length) };
        let str = std::str::from_utf8(slice);
        match str {
            Ok(str) => {
                println!("{}", str);
            },
            Err(_) => return -1
        };
    }
    1
}
